package mx.intellcomm.myapplication.data.model

import java.time.LocalDate


data class CursoDTO(val id: Int,
                    val nombre: String,
                    val descripcion: String,
                    val precio: Double,
                    val fechaInicio: LocalDate,
                    val fechaTermini: LocalDate,
                    val idProfesor: Int,
                    val profesor: String
)