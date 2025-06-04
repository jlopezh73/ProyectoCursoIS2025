package mx.intellcomm.myapplication.data

import android.content.Context
import mx.intellcomm.myapplication.data.model.CursoDTO
import mx.intellcomm.myapplication.data.model.PeticionInicioSesionDTO
import mx.intellcomm.myapplication.data.model.RespuestaValidacionUsuarioDTO
import mx.intellcomm.myapplication.data.model.UsuarioDTO
import mx.intellcomm.myapplication.service.CursosService
import mx.intellcomm.myapplication.service.LoginService
import java.io.IOException

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class CursosDataSource(val context: Context) {

    private val cursosService = CursosService.create(context)

    suspend fun obtenerCursos(): ApiResponse<List<CursoDTO>> {
        return try {
            val response = cursosService.obtenerCursos()
            if (response.isSuccessful) {
                ApiResponse.Success(response.body() ?: emptyList())
            } else {
                ApiResponse.Error(response.message())
            }
        } catch (e: Exception) {
            ApiResponse.Error(e.message)
        }
    }

    suspend fun obtenerCurso(id: Int): ApiResponse<CursoDTO> {
        return try {
            val response = cursosService.obtenerCurso(id)
            if (response.isSuccessful) {
                ApiResponse.Success(response.body()!!)
            } else {
                ApiResponse.Error(response.message())
            }
        } catch (e: Exception) {
            ApiResponse.Error(e.message)
        }
    }

    suspend fun agregarCurso(curso: CursoDTO): ApiResponse<Boolean> {
        return try {
            val response = cursosService.agregarCurso(curso)
            if (response.isSuccessful) {
                ApiResponse.Success(true)
            } else {
                ApiResponse.Error(response.message())
            }
        } catch (e: Exception) {
            ApiResponse.Error(e.message)
        }
    }

    suspend fun actualizarCurso(course: CursoDTO): ApiResponse<Boolean> {
        return try {
            val response = cursosService.actualizarCurso(course.id, course)
            if (response.isSuccessful) {
                ApiResponse.Success(true)
            } else {
                ApiResponse.Error(response.message())
            }
        } catch (e: Exception) {
            ApiResponse.Error(e.message)
        }
    }

    suspend fun eliminarCurso(id: Int): ApiResponse<Boolean> {
        return try {
            val response = cursosService.eliminarCurso(id)
            if (response.isSuccessful) {
                ApiResponse.Success(true)
            } else {
                ApiResponse.Error(response.message())
            }
        } catch (e: Exception) {
            ApiResponse.Error(e.message)
        }
    }
}