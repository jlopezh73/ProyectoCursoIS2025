package mx.intellcomm.myapplication.service

import android.content.Context
import android.content.Intent
import android.provider.SyncStateContract
import mx.intellcomm.myapplication.MainActivity
import mx.intellcomm.myapplication.data.SharedPreferencesHelper
import mx.intellcomm.myapplication.data.model.PeticionInicioSesionDTO
import mx.intellcomm.myapplication.data.model.RespuestaValidacionUsuarioDTO
import mx.intellcomm.myapplication.data.model.Constantes
import okhttp3.Interceptor
import okhttp3.logging.HttpLoggingInterceptor
import okhttp3.OkHttpClient
import okio.IOException
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT

interface LoginService {
    @POST("api/Identidad/validarUsuario")
    suspend fun login(@Body request: PeticionInicioSesionDTO): Response<RespuestaValidacionUsuarioDTO>


    companion object {
        fun create(context: Context): LoginService {
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
                .create(LoginService::class.java)
        }
    }
}

class AuthInterceptor(private val context: Context) : Interceptor {
    override fun intercept(chain: Interceptor.Chain): okhttp3.Response {
        val originalRequest = chain.request()

        // Skip auth for login endpoint
        if (originalRequest.url.encodedPath.contains("api/Identidad/validarUsuario")) {
            return chain.proceed(originalRequest)
        }

        val token = SharedPreferencesHelper.getAuthToken(context)
            ?: run {
                // Redirect to login if no token
                context.startActivity(Intent(context, MainActivity::class.java).apply {
                    flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                })
                    throw IOException("No auth token available")
            }

        val newRequest = originalRequest.newBuilder()
            .header("Authorization", "Bearer $token")
            .build()

        return chain.proceed(newRequest)
    }
}
