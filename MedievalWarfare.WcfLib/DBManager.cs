using MedievalWarfare.WcfLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib
{
    public class DBManager
    {
        string connString = @"Data Source=(localdb)\Projects;Initial Catalog=TEST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True";

        public void AddPlayer(MedievalWarfare.Common.Player player)
        {
            using (var ctx = new Context(connString))
            {
                Player p = new Player();
                p.Id = player.PlayerId;
                p.Name = player.Name;
                p.Neutral = player.Neutral;
                p.Gold = player.Gold;

                ctx.Players.Add(p);
                ctx.SaveChanges();


            }

        }

        public void RemovePlayer(Guid id)
        {
            using (var ctx = new Context(connString))
            {
                var p = from q in ctx.Players
                        where q.Id.Equals(id)
                        select q;
                ctx.Players.Remove(p.First());
                ctx.SaveChanges();
            }
        }

        public void AddCommand(MedievalWarfare.Common.Utility.Command command)
        {
            using (var ctx = new Context(connString))
            {


                Guid commandId = Guid.NewGuid();
                Guid targetID;
                Guid playerID;
                int x;
                int y;
                CommandType type;


                playerID = command.Player.PlayerId;

                if (command is MedievalWarfare.Common.Utility.MoveUnit)
                {
                    targetID = ((MedievalWarfare.Common.Utility.MoveUnit)command).Unit.Id;
                    x = ((MedievalWarfare.Common.Utility.MoveUnit)command).Position.X;
                    y = ((MedievalWarfare.Common.Utility.MoveUnit)command).Position.Y;
                    type = CommandType.MoveUnit;

                    Player p;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerID)
                                  select q;
                    if (players.Count() == 0)
                    {
                        p = new Player();
                        p.Id = playerID;
                        p.Gold = command.Player.Gold;
                        p.Neutral = command.Player.Neutral;
                        ctx.Players.Add(p);
                    }
                    else
                    {
                        p = players.First();
                    }

                    Unit u;

                    var units = from q in ctx.Units
                                where q.Id.Equals(targetID)
                                select q;
                    if (units.Count() == 0)
                    {
                        u = new Unit();
                        u.Id = targetID;
                        u.LocationX = ((MedievalWarfare.Common.Utility.MoveUnit)command).Unit.Tile.X;
                        u.LocationY = ((MedievalWarfare.Common.Utility.MoveUnit)command).Unit.Tile.Y;
                        u.owner = p;
                        u.Movement = ((MedievalWarfare.Common.Utility.MoveUnit)command).Unit.Movement;
                        u.Strength = ((MedievalWarfare.Common.Utility.MoveUnit)command).Unit.Strength;

                        ctx.Units.Add(u);
                    }
                    else
                    {
                        u = units.First();
                    }


                    Command com = new Command();
                    com.Id = commandId;
                    com.TargetX = x;
                    com.TargetY = y;
                    com.Type = type;
                    com.Owner = p;
                    com.TargetObject = u;

                    ctx.Commands.Add(com);


                }

                if (command is MedievalWarfare.Common.Utility.CreateUnit)
                {
                    targetID = ((MedievalWarfare.Common.Utility.CreateUnit)command).Unit.Id;
                    x = ((MedievalWarfare.Common.Utility.CreateUnit)command).Position.X;
                    y = ((MedievalWarfare.Common.Utility.CreateUnit)command).Position.Y;
                    type = CommandType.CreateUnit;

                    Player p;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerID)
                                  select q;
                    if (players.Count() == 0)
                    {
                        p = new Player();
                        p.Id = playerID;
                        p.Gold = command.Player.Gold;
                        p.Neutral = command.Player.Neutral;
                        ctx.Players.Add(p);
                    }
                    else
                    {
                        p = players.First();
                    }

                    Unit u;

                    var units = from q in ctx.Units
                                where q.Id.Equals(targetID)
                                select q;
                    if (units.Count() == 0)
                    {
                        u = new Unit();
                        u.Id = targetID;
                        u.LocationX = ((MedievalWarfare.Common.Utility.CreateUnit)command).Unit.Tile.X;
                        u.LocationY = ((MedievalWarfare.Common.Utility.CreateUnit)command).Unit.Tile.Y;
                        u.owner = p;
                        u.Movement = MedievalWarfare.Common.Utility.ConstantValues.BaseMovement;
                        u.Strength = MedievalWarfare.Common.Utility.ConstantValues.BaseUnitStr;

                        ctx.Units.Add(u);
                    }
                    else
                    {
                        u = units.First();
                    }

                    Command com = new Command();
                    com.Id = commandId;
                    com.TargetX = x;
                    com.TargetY = y;
                    com.Type = type;
                    com.Owner = p;
                    com.TargetObject = u;

                    ctx.Commands.Add(com);



                }

                if (command is MedievalWarfare.Common.Utility.ConstructBuilding)
                {
                    targetID = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Building.Id;
                    x = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Position.X;
                    y = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Position.Y;
                    type = CommandType.CreateBuilding;

                    Player p;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerID)
                                  select q;
                    if (players.Count() == 0)
                    {
                        p = new Player();
                        p.Id = playerID;
                        p.Gold = command.Player.Gold;
                        p.Neutral = command.Player.Neutral;
                        ctx.Players.Add(p);
                    }
                    else
                    {
                        p = players.First();
                    }

                    Building b;

                    var buildings = from q in ctx.Buildings
                                    where q.Id.Equals(targetID)
                                    select q;
                    if (buildings.Count() == 0)
                    {
                        b = new Building();
                        b.Id = targetID;
                        b.LocationX = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Building.Tile.X;
                        b.LocationY = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Building.Tile.Y;
                        b.owner = p;
                        b.Population = ((MedievalWarfare.Common.Utility.ConstructBuilding)command).Building.Population;

                        ctx.Buildings.Add(b);
                    }
                    else
                    {
                        b = buildings.First();
                    }

                    Command com = new Command();
                    com.Id = commandId;
                    com.TargetX = x;
                    com.TargetY = y;
                    com.Type = type;
                    com.Owner = p;
                    com.TargetObject = b;

                    ctx.Commands.Add(com);

                }

                ctx.SaveChanges();

            }
        }
        public void Flush()
        {
            using (var ctx = new Context(connString))
            {

                var all = from c in ctx.Players select c;
                ctx.Players.RemoveRange(all);

                var allunit = from c in ctx.Units select c;
                ctx.Units.RemoveRange(allunit);

                var allbuild = from c in ctx.Buildings select c;
                ctx.Buildings.RemoveRange(allbuild);

                var allCommand = from c in ctx.Commands select c;
                ctx.Commands.RemoveRange(allCommand);


                ctx.SaveChanges();

            }
        }

        public void PrintAllCommands()
        {
            using (var ctx = new Context(connString))
            {
                var cmds = from c in ctx.Commands select c;
                foreach (var command in cmds)
                {
                    Console.WriteLine(String.Format("Command Type: {0}; Command ID: {1}; User Name: {2}; Game Object ID: {3}; GO Coordinates: {4},{5}; Target Coordinates: {6},{7}",
                        command.Type, command.Id, command.Owner.Name,
                        command.TargetObject.Id, command.TargetObject.LocationX,
                        command.TargetObject.LocationY, command.TargetX, command.TargetY));
                }
            }
        }

    }
}
