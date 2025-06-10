package mx.intellcomm.myapplication.ui.cursos

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.ui.Modifier
import androidx.compose.ui.Alignment
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.rememberDrawerState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import androidx.compose.material3.FloatingActionButton
import androidx.compose.material3.Card
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Divider
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.DrawerValue
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ModalNavigationDrawer
import androidx.compose.material3.Text
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.platform.LocalContext
import mx.intellcomm.myapplication.data.model.CursoDTO
import androidx.compose.runtime.getValue

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ListaCursosScreen(
    viewModel: CursosViewModel = CursosViewModel(LocalContext.current),
    onEditarCurso: (Int) -> Unit,
    onLogout: () -> Unit
) {
    val scaffoldState = rememberDrawerState(initialValue = DrawerValue.Closed)
    val scope = rememberCoroutineScope()
    val cursos by viewModel.cursos.collectAsState()
    val isLoading by viewModel.isLoading.collectAsState()
    val error by viewModel.error.collectAsState()



    ModalNavigationDrawer(
        drawerContent = { DrawerContent(
            onCloseDrawer = { scope.launch { scaffoldState.close() } },
            onLogout = onLogout
        ) },
        drawerState = scaffoldState
    ) {
        Scaffold(
            topBar = {
                TopAppBar(
                    title = { Text("Mis Cursos") },
                    navigationIcon = {
                        IconButton(onClick = { scope.launch { scaffoldState.open() } }) {
                            Icon(Icons.Default.Menu, contentDescription = "Menú")
                        }
                    }
                )
            },

            floatingActionButton = {
                FloatingActionButton(onClick = { onEditarCurso(0) }) {
                    Icon(Icons.Default.Add, contentDescription = "Agregar curso")
                }
            }
        ) { padding ->
            Box(modifier = Modifier.padding(padding)) {
                if (isLoading) {
                    CircularProgressIndicator(modifier = Modifier.align(Alignment.Center))
                } else if (error != "") {
                    Text(
                        text = error!!,
                        modifier = Modifier.align(Alignment.Center),
                        color = MaterialTheme.colorScheme.error)
                } else {
                    LazyColumn(modifier = Modifier.fillMaxSize()) {
                        items(cursos) { curso: CursoDTO ->
                            CourseItem(
                                curso = curso,
                                onClick = { onEditarCurso(curso.id) }
                            )
                            Divider()
                        }
                    }
                }
            }
        }

    }
    LaunchedEffect(Unit) {
        viewModel.cargarCursos()
    }
}

@Composable
fun DrawerContent(
    onCloseDrawer: () -> Unit,
    onLogout: () -> Unit
) {
    Column(modifier = Modifier.fillMaxSize()) {
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .height(120.dp)
                .background(MaterialTheme.colorScheme.primary),
            contentAlignment = Alignment.Center
        ) {
            Text(
                text = "Menú",
                style = MaterialTheme.typography.labelMedium,
                color = MaterialTheme.colorScheme.onPrimary
            )
        }

        Spacer(modifier = Modifier.height(16.dp))

        Button(
            onClick = {
                onCloseDrawer()
                onLogout()
            },
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp)
        ) {
            Text("Cerrar Sesión")
        }
    }
}

@Composable
fun CourseItem(
    curso: CursoDTO,
    onClick: () -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .clickable(onClick = onClick)
            .padding(8.dp),

    ) {
        Column(modifier = Modifier.padding(16.dp)) {
            Text(
                text = curso.nombre,
                style = MaterialTheme.typography.labelMedium
            )
            Spacer(modifier = Modifier.height(4.dp))
            Text(
                text = curso.descripcion,
                style = MaterialTheme.typography.labelMedium
            )
            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = "$${curso.precio}",
                style = MaterialTheme.typography.titleMedium,
                color = MaterialTheme.colorScheme.primary
            )
        }
    }
}