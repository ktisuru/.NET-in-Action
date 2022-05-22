using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace WidgetScmDataAccess
{
    public class ScmContext
    {
        private DbConnection connection;

        public IEnumerable<PartType> Parts { get;  private set; }
        public IEnumerable<InventoryItem> Inventory { get; private set; }

        public ScmContext(DbConnection conn)
        {
            connection = conn;
            ReadParts();
            ReadInventory();
        }

        private void ReadParts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT Id, Name FROM partType";

                using (var reader = command.ExecuteReader())
                {
                    var parts = new List<PartType>();
                    Parts = parts;

                    while (reader.Read())
                    {
                        parts.Add(new PartType() { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }

                }
            }
        }

        private void ReadInventory()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT PartTypeId, Count, OrderThreshold FROM InventoryItem";

                using (var reader = command.ExecuteReader())
                {
                    var items = new List<InventoryItem>();
                    Inventory = items;

                    while (reader.Read())
                    {
                        var item = new InventoryItem() {
                            PartTypeId = reader.GetInt32(0),
                            Count = reader.GetInt32(1),
                            OrderThreshold = reader.GetInt32(2) 
                        };
                        items.Add(item);
                        item.Part = Parts.Single(p => p.Id == item.PartTypeId);

                    }
                }
            }
        }
        public void CreatePartCommand(PartCommand partCommand)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO PartCommand
                 (PartTypeId, Count, Command)
              VALUES
                 (@partTypeId, @partCount, @command); 
              SELECT last_insert_rowid();";

            AddParameter(command, "@partTypeId", partCommand.PartTypeId);
            AddParameter(command, "@partCount", partCommand.PartCount);
            AddParameter(command, "@command",partCommand.Command.ToString());
            long partCommandId = (long)command.ExecuteScalar();
            partCommand.Id = (int)partCommandId;
        }

    }
}
        