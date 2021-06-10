# mercadolibre
Repositorio de challenge mercadolibre

Para poder acceder a la api de xmen se debe acceder por medio de una URL http://ec2-34-205-216-4.compute-1.amazonaws.com/mutant


EL contenido del mensaje para el servicio POST debe ser el siguiente

{
“dna”:["ATGCGA","CAGTGC","TTATGT","AGAAGG","CCCCTA","TCACTG"]
}

El sistema devolvera true o false dependiendo si es mutante (true) o humano (false)

Para poder acceder a las estadisticas se debe ingresar a la siguiente URL

http://ec2-34-205-216-4.compute-1.amazonaws.com/stats

Devolvera una respuesta como la siguiente:

<StatsResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/Mercadolibre.Models">
    <count_human_dna>1</count_human_dna>
    <count_mutant_dna>2</count_mutant_dna>
</StatsResponse>

