using FundraisingandEngagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Tests
{
    public class PaymentContextSpy : PaymentContext
    {
        public static IModel GetModel()
        {
            return new PaymentContextSpy().BuiltModel;
        }

        private IModel BuiltModel
        {
            get
            {
                var model = new Model();
                OnModelCreating(new ModelBuilder(model));
                return model;
            }
        }
    }
}