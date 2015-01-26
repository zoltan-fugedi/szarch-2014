using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using MedievalWarfare.TestClient.Db.Entities;
using Building = MedievalWarfare.TestClient.Db.Entities.Building;
using Command = MedievalWarfare.TestClient.Db.Entities.Command;
using Player = MedievalWarfare.TestClient.Db.Entities.Player;
using Unit = MedievalWarfare.TestClient.Db.Entities.Unit;

namespace MedievalWarfare.TestClient.Db
{
    public class DbManager
    {
        string connString = @"Data Source=(localdb)\Projects;Initial Catalog=TEST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultipleActiveResultSets=True";

        public void AddPlayer(Common.Player player)
        {
            AddPlayerAsync(player).Wait();
        }

        public async Task AddPlayerAsync(Common.Player player)
        {
            using (var ctx = new Context(connString))
            {
                Player p = new Player();
                p.Id = player.PlayerId;
                p.Name = player.Name;
                p.Neutral = player.Neutral;
                p.Gold = player.Gold;

                ctx.Players.Add(p);
                await ctx.SaveChangesAsync();
            }
        }

        public void InitDb(Map map)
        {
            InitDbAsync(map).Wait();
        }

        public async Task InitDbAsync(Map map)
        {
            await this.FlushAsync();

            foreach (var player in map.Game.Players)
            {
                await this.AddPlayerAsync(player);
            }
        }

        public void RemovePlayer(Guid id)
        {
            RemovePlayerAsync(id).Wait();
        }

        public async Task RemovePlayerAsync(Guid id)
        {
            using (var ctx = new Context(connString))
            {
                var p = from q in ctx.Players
                        where q.Id.Equals(id)
                        select q;
                ctx.Players.Remove(p.First());
                await ctx.SaveChangesAsync();
            }
        }

        public void AddCommand(Common.Utility.Command command)
        {
            AddCommandAsync(command).Wait();
        }

        public async Task AddCommandAsync(Common.Utility.Command command)
        {
            using (var ctx = new Context(connString))
            {
                Guid commandId = Guid.NewGuid();
                Guid targetId;
                Guid playerId = command.Player.PlayerId; ;
                int x;
                int y;
                CommandType type;

                if (command is MoveUnit)
                {
                    targetId = ((MoveUnit)command).Unit.Id;
                    x = ((MoveUnit)command).Position.X;
                    y = ((MoveUnit)command).Position.Y;
                    type = CommandType.MoveUnit;

                    Player player;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerId)
                                  select q;

                    if (!players.Any())
                    {
                        player = new Player
                        {
                            Id = playerId,
                            Gold = command.Player.Gold,
                            Neutral = command.Player.Neutral
                        };
                        ctx.Players.Add(player);
                    }
                    else
                    {
                        player = players.First();
                    }

                    Unit unit;
                    var units = from q in ctx.Units
                                where q.Id.Equals(targetId)
                                select q;

                    if (!units.Any())
                    {
                        unit = new Unit
                        {
                            Id = targetId,
                            LocationX = ((MoveUnit)command).Unit.Tile.X,
                            LocationY = ((MoveUnit)command).Unit.Tile.Y,
                            owner = player,
                            Movement = ((MoveUnit)command).Unit.Movement,
                            Strength = ((MoveUnit)command).Unit.Strength
                        };

                        ctx.Units.Add(unit);
                    }
                    else
                    {
                        unit = units.First();
                    }

                    var com = new Command
                    {
                        Id = commandId,
                        TargetX = x,
                        TargetY = y,
                        Type = type,
                        Owner = player,
                        TargetObject = unit
                    };

                    ctx.Commands.Add(com);
                }

                if (command is CreateUnit)
                {
                    targetId = ((CreateUnit)command).Unit.Id;
                    x = ((CreateUnit)command).Position.X;
                    y = ((CreateUnit)command).Position.Y;
                    type = CommandType.CreateUnit;

                    Player player;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerId)
                                  select q;

                    if (!players.Any())
                    {
                        player = new Player
                        {
                            Id = playerId,
                            Gold = command.Player.Gold,
                            Neutral = command.Player.Neutral,
                        };

                        ctx.Players.Add(player);
                    }
                    else
                    {
                        player = players.First();
                    }

                    Unit unit;
                    var units = from q in ctx.Units
                                where q.Id.Equals(targetId)
                                select q;

                    if (!units.Any())
                    {
                        unit = new Unit
                        {
                            Id = targetId,
                            LocationX = ((CreateUnit)command).Unit.Tile.X,
                            LocationY = ((CreateUnit)command).Unit.Tile.Y,
                            owner = player,
                            Movement = ConstantValues.BaseMovement,
                            Strength = ConstantValues.BaseUnitStr,
                        };


                        ctx.Units.Add(unit);
                    }
                    else
                    {
                        unit = units.First();
                    }

                    var com = new Command
                    {
                        Id = commandId,
                        TargetX = x,
                        TargetY = y,
                        Type = type,
                        Owner = player,
                        TargetObject = unit,
                    };
                    ctx.Commands.Add(com);
                }

                if (command is ConstructBuilding)
                {
                    targetId = ((ConstructBuilding)command).Building.Id;
                    x = ((ConstructBuilding)command).Position.X;
                    y = ((ConstructBuilding)command).Position.Y;
                    type = CommandType.CreateBuilding;

                    Player player;
                    var players = from q in ctx.Players
                                  where q.Id.Equals(playerId)
                                  select q;

                    if (!players.Any())
                    {
                        player = new Player
                        {
                            Id = playerId,
                            Gold = command.Player.Gold,
                            Neutral = command.Player.Neutral,
                        };

                        ctx.Players.Add(player);
                    }
                    else
                    {
                        player = players.First();
                    }

                    Building building;
                    var buildings = from q in ctx.Buildings
                                    where q.Id.Equals(targetId)
                                    select q;

                    if (!buildings.Any())
                    {
                        building = new Building
                        {
                            Id = targetId,
                            LocationX = ((ConstructBuilding)command).Building.Tile.X,
                            LocationY = ((ConstructBuilding)command).Building.Tile.Y,
                            owner = player,
                            Population = ((ConstructBuilding)command).Building.Population
                        };

                        ctx.Buildings.Add(building);
                    }
                    else
                    {
                        building = buildings.First();
                    }

                    var com = new Command
                    {
                        Id = commandId,
                        TargetX = x,
                        TargetY = y,
                        Type = type,
                        Owner = player,
                        TargetObject = building
                    };

                    ctx.Commands.Add(com);
                }

                await ctx.SaveChangesAsync();
            }
        }

        public void Flush()
        {
            FlushAsync().Wait();
        }
        
        public async Task FlushAsync()
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

                await ctx.SaveChangesAsync();
            }
        }

        public List<string> PrintAllCommands()
        {
            using (var ctx = new Context(connString))
            {
                var retVal = new List<string>();
                var cmds = from c in ctx.Commands select c;
                foreach (var command in cmds)
                {
                    retVal.Add( String.Format("Command Type: {0}; Command ID: {1}; User Name: {2}; Game Object ID: {3}; GO Coordinates: {4},{5}; Target Coordinates: {6},{7}",
                        command.Type, command.Id, command.Owner.Name,
                        command.TargetObject.Id, command.TargetObject.LocationX,
                        command.TargetObject.LocationY, command.TargetX, command.TargetY));
                }

                return retVal;
            }
        }

    }
}
