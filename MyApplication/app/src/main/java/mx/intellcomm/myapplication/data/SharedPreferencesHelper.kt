package mx.intellcomm.myapplication.data

import android.content.Context

object SharedPreferencesHelper {
    private const val PREFS_NAME = "app_prefs"
    private const val AUTH_TOKEN_KEY = "token"

    fun saveAuthToken(context: Context, token: String) {
        context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
            .edit()
            .putString(AUTH_TOKEN_KEY, token)
            .apply()
    }

    fun getAuthToken(context: Context): String? {
        return context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
            .getString(AUTH_TOKEN_KEY, null)
    }

    fun clearAuthToken(context: Context) {
        context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)
            .edit()
            .remove(AUTH_TOKEN_KEY)
            .apply()
    }
}