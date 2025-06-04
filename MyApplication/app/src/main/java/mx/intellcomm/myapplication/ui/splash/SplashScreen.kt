package mx.intellcomm.myapplication.ui.splash

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
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
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import mx.intellcomm.myapplication.R

@Composable
fun SplashScreen(navController: NavController) {
    val scope = rememberCoroutineScope()

    LaunchedEffect(Unit) {
        scope.launch {
            delay(3000)
            navController.navigate("login")
        }
    }

    Column(
        modifier = Modifier.fillMaxSize()
            .background(
                Brush.linearGradient(
                    0.0f to Color(0,148,56),
                    0.6f to Color(10,83,159),
                    0.6f to Color(103,103,103),
                    0.61f to Color(232,232,232),
                    1.0f to Color(255,255,255),
                    end = Offset.Zero,
                    start = Offset.Infinite,

                    )),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Top,
    ) {
        Spacer(modifier = Modifier.height(50.dp))

        Image(painter = painterResource(R.drawable.uv),
            contentDescription = "uv",
            contentScale = ContentScale.FillHeight,
            modifier = Modifier.height(150.dp)
        )
        Spacer(modifier = Modifier.height(150.dp))
        Image(painter = painterResource(R.drawable.logo),
            contentDescription = "Logo",
            modifier = Modifier.height(300.dp))
        Spacer(modifier = Modifier.height(20.dp))
        Text("Sistema Institucional para el Registro de Cursos de Educación Contínua",
            fontSize = 26.sp,
            textAlign = TextAlign.Center,
            color = Color.White,
            fontWeight = FontWeight.Medium, modifier = Modifier.padding(horizontal = 10.dp))

    }
}
