package mx.intellcomm.myapplication

import android.annotation.SuppressLint
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import mx.intellcomm.myapplication.ui.cursos.ListaCursosScreen
import mx.intellcomm.myapplication.ui.theme.CourseApplicationTheme
import mx.intellcomm.myapplication.ui.login.LoginScreen
import mx.intellcomm.myapplication.ui.splash.SplashScreen
import mx.intellcomm.myapplication.ui.edicion_cursos.EdicionCursosScreen


class MainActivity : ComponentActivity() {
    @SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            CourseApplicationTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) {
                    AppNavigation()
                }
            }
        }
    }
}



@Composable
fun AppNavigation()  {
    val navController = rememberNavController()
    val context = LocalContext.current

    NavHost(navController, startDestination = Destinos.Splash.ruta) {
        composable(Destinos.Splash.ruta) { SplashScreen(navController = navController) }
        composable(Destinos.Login.ruta) {
            LoginScreen(
                onLoginSuccess = {
                    navController.navigate(Destinos.Cursos.ruta) {
                        popUpTo(Destinos.Login.ruta) { inclusive = true }
                    }
                }
            )
        }
        composable(Destinos.Cursos.ruta) {
            ListaCursosScreen(
                onEditarCurso = { cursoId ->
                    navController.navigate("${Destinos.EdicionCursos.ruta}/$cursoId")
                },
                onLogout = {
                    navController.navigate(Destinos.Login.ruta) {
                        popUpTo(Destinos.Cursos.ruta) { inclusive = true }
                    }
                }
            )
        }

        composable(
            route = "${Destinos.EdicionCursos.ruta}/{cursoId}",
            arguments = listOf(navArgument("cursoId") {
                type = NavType.IntType
                nullable = false
                defaultValue = 0
            })
        ) { backStackEntry ->
            val cursoId = backStackEntry.arguments?.getInt("cursoId")
            EdicionCursosScreen(
                cursoId = cursoId,
                onBack = { navController.popBackStack() }
            )
        }
    }
}

object Destinos {
    object Splash : Destino {
        override val ruta = "splash"
    }

    object Login : Destino {
        override val ruta = "login"
    }

    object Cursos : Destino {
        override val ruta = "cursos"
    }

    object EdicionCursos : Destino {
        override val ruta = "cursos/edicion"
        const val ARG_COURSE_ID = "cursoId"
        fun crearRuta(cursoId: Int?) = "$ruta/${cursoId ?: ""}"
    }
}

interface Destino {
    val ruta: String
}

@Preview(showBackground = true)
@Composable
fun NavigationPreview() {
    CourseApplicationTheme {
        AppNavigation()
        //Greeting("Android")
        //LoginActivity(onLoginSuccess = {})
    }
}