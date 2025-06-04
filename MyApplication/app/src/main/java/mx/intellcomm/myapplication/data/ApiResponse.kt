package mx.intellcomm.myapplication.data

sealed class ApiResponse<out T> {
    data class Success<out T>(val data: T) : ApiResponse<T>()
    data class Error(val message: String?) : ApiResponse<Nothing>()

    val isSuccess: Boolean get() = this is Success<T>
}