package mx.intellcomm.myapplication.data

import android.content.Context
import mx.intellcomm.myapplication.data.model.CursoDTO
import mx.intellcomm.myapplication.service.CursosService

class CursosRepository(val context: Context, val dataSource: CursosDataSource = CursosDataSource(context)) {
    private val cursosService = CursosService.create(context)

    suspend fun obtenerCursos(): List<CursoDTO> {
        return try {
            val response = dataSource.obtenerCursos()

            if (response is ApiResponse.Success<List<CursoDTO>>) {
                response.data
            } else {
                emptyList<CursoDTO>()
            }
        } catch (e: Exception) {
            emptyList<CursoDTO>()
        }
    }

    suspend fun obtenerCurso(id: Int): CursoDTO? {
        return try {
            val response = dataSource.obtenerCurso(id)
            if (response is ApiResponse.Success<CursoDTO>) {
                response.data
            } else {
                return null
            }
        } catch (e: Exception) {
            return null
        }
    }

    suspend fun agregarCurso(curso: CursoDTO): Boolean {
        return try {
            val response = dataSource.agregarCurso(curso)
            if (response is ApiResponse.Success<Boolean>) {
                true
            } else {
                false
            }
        } catch (e: Exception) {
            false
        }
    }

    suspend fun actualizarCurso(curso: CursoDTO): Boolean {
        return try {
            val response = dataSource.actualizarCurso(curso)
            if (response is ApiResponse.Success<Boolean>) {
                true
            } else {
                false
            }
        } catch (e: Exception) {
            false
        }
    }

    suspend fun eliminarCurso(id: Int): Boolean {
        return try {
            val response = dataSource.eliminarCurso(id)
            if (response is ApiResponse.Success<Boolean>) {
                true
            } else {
                false
            }
        } catch (e: Exception) {
            false
        }
    }
}