﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop.Model.Model
{
    [Table("VistitorStatitics")]
    class VisitorStatitic
    {
        [Key]
        public Guid ID { set; get; }
        public DateTime VisitedDate { set; get; }
        [MaxLength(50)]
        public string IDAddress { set; get; }
    }
}
