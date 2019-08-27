using System;
using System.ComponentModel.DataAnnotations;

namespace AppEventGrid.Model
{
    public class Cliente
    {
        [Key]
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime DataModificacao { get; set; }

    }
}
