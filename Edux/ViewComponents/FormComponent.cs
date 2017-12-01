﻿using Edux.Data;
using Edux.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edux.ViewComponents
{
    public class FormComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public FormComponent(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Models.Component component)
        {
            var viewName = component.View ?? "Default";
            var formName = component.ParameterValues.FirstOrDefault(f => f.Parameter.Name == "FormName")?.Value;
            var initialValues = component.ParameterValues.FirstOrDefault(f => f.Parameter.Name == "InitialValues")?.Value;
            if (initialValues != null) { 
                ViewBag.InitialValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(initialValues);
            }
            if (String.IsNullOrEmpty(Request.Query["returnUrl"].ToString())) {
                ViewBag.ReturnUrl = Request.Path;
            } else
            {
                ViewBag.ReturnUrl = Request.Query["returnUrl"].ToString();
            }
            string mode = Request.Query["mode"].ToString().ToLowerInvariant();
            if (String.IsNullOrEmpty(mode))
            {
                mode = "create";
            }
            ViewBag.Mode = mode;
            var rowId = Request.Query["id"].ToString();
            ViewBag.RowId = rowId;
            var frm = await _context.Forms.Include(t=>t.Tabs).ThenInclude(f => f.Fields).ThenInclude(ff => ff.Property).ThenInclude(d=>d.DataSourceProperty).Include("Fields.Property.PropertyValues").SingleOrDefaultAsync(f => f.Name == formName);
            if (frm == null)
            {
                throw new Exception($"\"{formName}\"adında bir form bulunamadı.");
            }
            ViewBag.Form = frm;
            var formEntityId = ((Form)ViewBag.Form).EntityId;
            if ((mode == "edit" || mode == "delete") && !String.IsNullOrEmpty(rowId))
            {
                ViewBag.RowValues = _context.PropertyValues.Include(pv => pv.Entity).Include(pv => pv.Property).ThenInclude(p => p.DataSourceProperty).ThenInclude(v => v.PropertyValues).Include("Property.DataSourceEntity").Where(pv => pv.EntityId == formEntityId && pv.RowId == Convert.ToInt64(rowId)).OrderBy(r => r.RowId).ToList();
               
            }
            IDictionary<String, IList<PropertyValue>> DataSourcePropertyValues = new Dictionary<string, IList<PropertyValue>>();
            foreach (var item in ((Form)ViewBag.Form).Fields)
            {
                if (item.Property.DataSourceProperty != null && !String.IsNullOrEmpty(item.Property.DataSourceProperty.Id))
                {
                    var entityId = item.Property.DataSourceProperty.EntityId;
                    var pvs = _context.PropertyValues.Where(pv => pv.Entity.Id == entityId && pv.PropertyId == item.Property.DataSourceProperty.Id).OrderBy(r => r.RowId).ToList();
                    if (!DataSourcePropertyValues.ContainsKey(item.Property.DataSourceProperty.Id)) { 
                        DataSourcePropertyValues.Add(item.Property.DataSourceProperty.Id, pvs);
                    }
                }
            }
            ViewBag.DataSourcePropertyValues = DataSourcePropertyValues;
            return await Task.FromResult(View(viewName, component));
        }
    }
}
