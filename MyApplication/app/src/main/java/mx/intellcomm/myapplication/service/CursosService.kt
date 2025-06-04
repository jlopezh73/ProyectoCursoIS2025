package mx.intellcomm.myapplication.service

import android.content.Context
import mx.intellcomm.myapplication.data.model.Constantes
import mx.intellcomm.myapplication.data.model.CursoDTO
import okhttp3.logging.HttpLoggingInterceptor
import okhttp3.OkHttpClient
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path

interface CursosService {

    @GET("courses")
    suspend fun obtenerCursos(): Response<List<CursoDTO>>

    @GET("courses/{id}")
    suspend fun obtenerCurso(@Path("id") id: Int): Response<CursoDTO>

    @POST("courses")
    suspend fun agregarCurso(@Body course: CursoDTO): Response<CursoDTO>

    @PUT("courses/{id}")
    suspend fun actualizarCurso(@Path("id") id: Int, @Body course: CursoDTO): Response<CursoDTO>

    @DELETE("courses/{id}")
    suspend fun eliminarCurso(@Path("id") id: Int): Response<Void>

    companion object {
        fun create(context: Context): CursosService {
            val okHttpClient = OkHttpClient.Builder()
                .addInterceptor(AuthInterceptor(context))
                .addInterceptor(HttpLoggingInterceptor().apply {
                    level = HttpLoggingInterceptor.Level.BODY
                })
                .build()

            return Retrofit.Builder()
                .baseUrl(Constantes.API_BASE_URL)
                .client(okHttpClient)
                .addConverterFactory(GsonConverterFactory.create())
                .build()
                .create(CursosService::class.java)
        }
    }
}