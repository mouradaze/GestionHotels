//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestHot.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {

        public Comment() { }
        public Comment(int idU, int idH, string message)
        {
            
            this.message = message;
            this.idU = idU;
            this.idH = idH;
        }

        public int idC { get; set; }
        public string message { get; set; }
        public int idU { get; set; }
        public int idH { get; set; }
        public Nullable<int> prediction { get; set; }
    
        public virtual Hotel Hotel { get; set; }
        public virtual User User { get; set; }
    }
}
