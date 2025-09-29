using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighgeekNet.Common.Server.Data.Models.mcwebapp_application
{
    public class MinecraftUser
    {
        public MinecraftUser(string uuid, string name)
        {
            Uuid = uuid;
            Name = name;
        }

        [Key]
        public string Uuid { get; set; }
        public string Name { get; set; }
    }
}
