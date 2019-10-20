using System;
using System.Collections.Generic;
using System.Text;

namespace Chatting.Infrastructure
{
   public class DatabaseSetting
   {
      public string ConnectionString { get; set; }

      public string DatabaseName { get; set; }
   }

   public class ReadOnlyDatabaseSetting
   {
      public string ConnectionString { get; set; }

      public string DatabaseName { get; set; }
   }

   public class MasterDatabaseSetting
   {
      public string ConnectionString { get; set; }
   }
}
