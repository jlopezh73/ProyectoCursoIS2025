package mx.intellcomm.myapplication.ui.cursos

import android.content.Context
import androidx.compose.runtime.State
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.SharedFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import mx.intellcomm.myapplication.data.CursosRepository
import mx.intellcomm.myapplication.data.model.CursoDTO
import java.time.LocalDate

class CursosViewModel(context: Context) : ViewModel() {
    private val repository = CursosRepository(context)

    private val _cursos = MutableStateFlow<List<CursoDTO>>(emptyList())
    val cursos: StateFlow<List<CursoDTO>> get() = _cursos

    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> get()  = _isLoading

    private val _error = MutableStateFlow<String>("")
    val error: StateFlow<String> get() = _error

    private val _edicionState = MutableStateFlow(EdicionCursoState())
    val edicionState: StateFlow<EdicionCursoState> get() = _edicionState

    private val _exitoGuardar = MutableSharedFlow<Unit>()
    val exitoGuardar: SharedFlow<Unit> get() = _exitoGuardar

    fun cargarCursos() {

        viewModelScope.launch {
            _isLoading.value = true
            _error.value = ""

            try {
                val response = repository.obtenerCursos()
                if (response.isEmpty()) {
                    _cursos.value = response
                } else {
                    _cursos.value = emptyList()
                    _error.value =  "Error al cargar cursos"
                }
            } catch (e: Exception) {
                _error.value = e.message ?: "Error de conexión"
            } finally {
                _isLoading.value = false
            }
        }
    }

    fun cargarCurso(id: Int) {
        viewModelScope.launch {
            _edicionState.value = _edicionState.value.copy(isLoading = true)

            try {
                val response = repository.obtenerCurso(id)
                if (response != null) {
                    response?.let { curso: CursoDTO ->
                        _edicionState.value = _edicionState.value.copy(
                            id = curso.id,
                            nombre = curso.nombre,
                            descripcion = curso.descripcion,
                            precio = curso.precio.toString(),
                            fechaInicio = curso.fechaInicio.toString(),
                            fechaTermino = curso.fechaTermini.toString(),
                            profesor = curso.profesor,
                            idProfesor = curso.idProfesor,
                            isLoading = false
                        )
                    }
                } else {
                    _edicionState.value = _edicionState.value.copy(
                        error =  "Error al cargar el curso",
                        isLoading = false
                    )
                }
            } catch (e: Exception) {
                _edicionState.value = _edicionState.value.copy(
                    error = e.message ?: "Error de conexión",
                    isLoading = false
                )
            }
        }
    }

    fun onNombreChange(nombre: String) {
        _edicionState.value = _edicionState.value.copy(
            nombre = nombre,
            nombreError = null
        )
    }

    fun onDescripcionChange(descripcion: String) {
        _edicionState.value = _edicionState.value.copy(
            descripcion = descripcion,
            descripcionError = null
        )
    }

    fun onPriceChange(precio: String) {
        _edicionState.value = _edicionState.value.copy(
            precio = precio,
            precioError = null
        )
    }

    fun guardarCurso() {
        if (!validateInputs()) return

        viewModelScope.launch {
            _edicionState.value = _edicionState.value.copy(isLoading = true, error = null)

            try {
                val course = CursoDTO(
                    id = _edicionState.value.id ?: 0,
                    nombre = _edicionState.value.nombre,
                    descripcion = _edicionState.value.descripcion,
                    precio = _edicionState.value.precio.toDouble(),
                    fechaInicio = LocalDate.parse(edicionState.value.fechaInicio),
                    fechaTermini = LocalDate.parse(edicionState.value.fechaTermino),
                    profesor = edicionState.value.profesor,
                    idProfesor = edicionState.value.idProfesor
                )

                val response = if (_edicionState.value.id == null) {
                    repository.agregarCurso(course)
                } else {
                    repository.actualizarCurso(course)
                }

                if (response) {
                    _exitoGuardar.emit(Unit)
                    cargarCursos()
                } else {
                    _edicionState.value = _edicionState.value.copy(
                        error = "Error al guardar el curso",
                        isLoading = false
                    )
                }
            } catch (e: Exception) {
                _edicionState.value = _edicionState.value.copy(
                    error = e.message ?: "Error de conexión",
                    isLoading = false
                )
            }
        }
    }

    fun eliminarCurso() {
        _edicionState.value.id?.let { id ->
            viewModelScope.launch {
                _edicionState.value = _edicionState.value.copy(isLoading = true)

                try {
                    val response = repository.eliminarCurso(id)
                    if (response) {
                        cargarCursos()
                    }
                } catch (e: Exception) {
                    // Handle error if needed
                }
            }
        }
    }

    private fun validateInputs(): Boolean {
        var isValid = true

        if (_edicionState.value.nombre.isEmpty()) {
            _edicionState.value = _edicionState.value.copy(
                nombreError = "Nombre requerido"
            )
            isValid = false
        }

        if (_edicionState.value.descripcion.isEmpty()) {
            _edicionState.value = _edicionState.value.copy(
                descripcionError = "Descripción requerida"
            )
            isValid = false
        }

        try {
            _edicionState.value.precio.toDouble()
        } catch (e: Exception) {
            _edicionState.value = _edicionState.value.copy(
                precioError = "Precio inválido"
            )
            isValid = false
        }

        return isValid
    }
}

data class EdicionCursoState(
    val id: Int? = null,
    val nombre: String = "",
    val descripcion: String = "",
    val precio: String = "",
    val fechaInicio: String = "",
    val fechaTermino: String = "",
    val idProfesor: Int = 0,
    val profesor: String = "",

    val isLoading: Boolean = false,
    val nombreError: String? = null,
    val descripcionError: String? = null,
    val precioError: String? = null,
    val fechaInicioError: String? = null,
    val fechaTerminoError: String? = null,
    val profesorError: String? = null,
    val error: String? = null
)