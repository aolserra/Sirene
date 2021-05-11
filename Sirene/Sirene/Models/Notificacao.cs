using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sirene.Models
{
    public class Notificacao
    {
        public string Id { get; set; }

        public string GroupName { get; set; }

        public string UserId { get; set; }

        public string UserJason { get; set; }

        [NotMapped]
        public Usuario User { get; set; }

        public string MsgType { get; set; }

        public string Text { get; set; }

        public bool Completed { get; set; }

        public string NotificationType { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}