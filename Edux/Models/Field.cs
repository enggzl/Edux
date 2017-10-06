﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edux.Models
{
    public class Field:BaseEntity
    {
        [Required]
        [Display(Name = "Alan Adı")]
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        [Required]
        [Display(Name = "Alan Görünen Adı")]
        public string DisplayName { get; set; }
        [Display(Name = "Form")]
        public string FormId { get; set; }
        [Display(Name = "Form")]
        [ForeignKey("FormId")]
        public Form Form { get; set; }
        [Display(Name = "Varlık")]
        public string EntityId { get; set; }
        [Display(Name = "Varlık")]
        [ForeignKey("EntityId")]
        public Entity Entity { get; set; }
        [Display(Name = "Özellik")]
        public string PropertyId { get; set; }
        [Display(Name = "Özellik")]
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        public string TabId { get; set; }
        [ForeignKey("TabId")]
        public Tab Tab { get; set; }
        [Display(Name = "Satır")]
        public int Row { get; set; }
        [Display(Name = "Sütun")]
        public int Col { get; set; }
        [Display(Name = "Pozisyon")]
        public int Position { get; set; }
        [Display(Name = "Editör Türü")]
        public EditorType EditorType { get; set; }
        [Display(Name = "Varsayılan Değer")]
        public string DefaultValue { get; set; }
        [StringLength(200)]
        [Display(Name = "Seçenek Etiketi")]
        public string OptionLabel { get; set; }
        [StringLength(200)]
        public string OnChange { get; set; }
        [StringLength(200)]
        public string OnClick { get; set; }
        [StringLength(200)]
        public string CssClass { get; set; }
        public bool IsReadOnly { get; set; }
        public string VisibleToRoles { get; set; }
        public string ReadOnlyToRoles { get; set; }
        [StringLength(200)]
        public string FieldSet { get; set; }
        [StringLength(200)]
        public string DataTableId { get; set; }
        [ForeignKey("DataTableId")]
        public DataTable DataTable { get; set; }
    }
}
