﻿using System.Text;

namespace Bringly.Data
{
    public partial class BringlyEntities
    {
        public override int SaveChanges()
        {
            try
            {   
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder st = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    st.Append(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        st.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                Utilities.ErrorLog.LogError(st.ToString(), "OdinEntities - SaveChanges");
                // throw;
                return 0;
            }
        }
    }
}
