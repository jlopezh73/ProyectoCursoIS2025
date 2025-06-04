package mx.intellcomm.myapplication.ui.login

import android.content.Context
import android.util.Patterns
import androidx.compose.runtime.State
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.SharedFlow
import kotlinx.coroutines.launch
import mx.intellcomm.myapplication.data.LoginRepository

class LoginViewModel(val context: Context) : ViewModel() {
    private val repository = LoginRepository(context)

    private val _state = mutableStateOf(LoginState())
    val state: State<LoginState> = _state

    private val _loginSuccess = MutableSharedFlow<Unit>()
    val loginSuccess: SharedFlow<Unit> = _loginSuccess

    fun onEmailChange(email: String) {
        _state.value = _state.value.copy(
            email = email,
            emailError = null
        )
    }

    fun onPasswordChange(password: String) {
        _state.value = _state.value.copy(
            password = password,
            passwordError = null
        )
    }

    fun login() {
        if (!validateInputs()) return

        viewModelScope.launch {
            _state.value = _state.value.copy(isLoading = true, error = null)

            try {
                val response = repository.login(
                    username = _state.value.email,
                    password = _state.value.password
                )

                if (response.correcto) {
                    _loginSuccess.emit(Unit)
                } else {
                    _state.value = _state.value.copy(
                        error =  "Credenciales erroneas"
                    )
                }
            } catch (e: Exception) {
                _state.value = _state.value.copy(
                    error = e.message ?: "Error de conexión"
                )
            } finally {
                _state.value = _state.value.copy(isLoading = false)
            }
        }
    }

    private fun validateInputs(): Boolean {
        val email = _state.value.email
        val password = _state.value.password

        var isValid = true

        if (email.isEmpty() || !Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            _state.value = _state.value.copy(
                emailError = "Ingrese un email válido"
            )
            isValid = false
        }

        if (password.isEmpty() || password.length < 3) {
            _state.value = _state.value.copy(
                passwordError = "La contraseña debe tener al menos 6 caracteres"
            )
            isValid = false
        }

        return isValid
    }
}

data class LoginState(
    val email: String = "administrador@cursos.uv.mx",
    val password: String = "123",
    val isLoading: Boolean = false,
    val emailError: String? = null,
    val passwordError: String? = null,
    val error: String? = null
)