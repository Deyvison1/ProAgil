using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int Id                                         { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(100, MinimumLength=3,ErrorMessage="Local é entre 3 a 100 Caracteres")]
        public string Local                                   { get; set; }
        public DateTime DataEvento                              { get; set; }
        public string Tema                                    { get; set; }
        [Range(2, 120000, ErrorMessage = "Quantidade de pessoas deve ser entre 2 a 120000")]
        public int QtdPessoas                                 { get; set; }
        public string ImagemURL                               { get; set; }
        [Phone]
        public string Telefone                                { get; set; }
        [EmailAddress]
        public string Email                                   { get; set; }
        public string Lote                                    { get; set; }
        public List<LoteDto> Lotes                            { get; set; }
        public List<RedeSocialDto> RedesSociais               { get; set; }
        public List<PalestranteDto> Palestrantes              { get; set; }   
    }
}