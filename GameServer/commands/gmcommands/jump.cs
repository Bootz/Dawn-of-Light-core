/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;
using DOL.GS.PacketHandler;
using DOL.Language;

namespace DOL.GS.Commands
{
	[CmdAttribute("&jump",
		ePrivLevel.GM,
		"GMCommands.Jump.Description",
		"GMCommands.Jump.Information",
		"GMCommands.Jump.Usage.ToPlayerName",
		"GMCommands.Jump.Usage.ToNameRealmID",
		"GMCommands.Jump.Usage.ToXYZRegionID",
		"GMCommands.Jump.Usage.PlayerNameToXYZ",
		"GMCommands.Jump.Usage.PlayerNameToXYZRegID",
		"GMCommands.Jump.Usage.PlayerNToPlayerN",
		"GMCommands.Jump.Usage.ToGT",
		"GMCommands.Jump.Usage.RelXYZ")]
	public class JumpCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			#region Jump to GT
			if (args.Length == 3 && args[1].ToLower() == "to" && args[2].ToLower() == "gt")
			{
				client.Player.MoveTo(client.Player.CurrentRegionID, client.Player.GroundTarget.X, client.Player.GroundTarget.Y, client.Player.GroundTarget.Z, client.Player.Heading);
				return;
			}
			#endregion Jump to GT
			#region Jump to PlayerName
			if (args.Length == 3 && args[1] == "to")
			{
				GameClient clientc;
				clientc = WorldMgr.GetClientByPlayerName(args[2], false, true);
				if (clientc == null)
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.CannotBeFound", args[2]), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				if (CheckExpansion(client, clientc, clientc.Player.CurrentRegionID))
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.JumpToX", clientc.Player.CurrentRegion.Description), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					if (clientc.Player.CurrentHouse != null)
						clientc.Player.CurrentHouse.Enter(client.Player);
					else
						client.Player.MoveTo(clientc.Player.CurrentRegionID, clientc.Player.X, clientc.Player.Y, clientc.Player.Z, client.Player.Heading);
					return;
				}
				return;
			}
			#endregion Jump to PlayerName
			#region Jump to Name Realm
			else if (args.Length == 4 && args[1] == "to")
			{
				GameClient clientc;
				clientc = WorldMgr.GetClientByPlayerName(args[2], false, true);
				if (clientc == null)
				{
					int realm = int.Parse(args[3]);

					GameNPC[] npcs = WorldMgr.GetNPCsByName(args[2], (eRealm)realm);
					if (npcs.Length > 0)
					{
						client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.JumpToX", npcs[0].CurrentRegion.Description), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						client.Player.MoveTo(npcs[0].CurrentRegionID, npcs[0].X, npcs[0].Y, npcs[0].Z, npcs[0].Heading);
						return;
					}

					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.CannotBeFoundInRealm", args[2], realm), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				if (CheckExpansion(client, clientc, clientc.Player.CurrentRegionID))
				{
					if (clientc.Player.InHouse)
					{
						client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.CannotJumpToInsideHouse"), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						return;
					}
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.JumpToX", clientc.Player.CurrentRegion.Description), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					if (clientc.Player.CurrentHouse != null)
						clientc.Player.CurrentHouse.Enter(client.Player);
					else
						client.Player.MoveTo(clientc.Player.CurrentRegionID, clientc.Player.X, clientc.Player.Y, clientc.Player.Z, client.Player.Heading);
					return;
				}
				return;
			}
			#endregion Jump to Name Realm
			#region Jump to X Y Z
			else if (args.Length == 5 && args[1] == "to")
			{
				client.Player.MoveTo(client.Player.CurrentRegionID, Convert.ToInt32(args[2]), Convert.ToInt32(args[3]), Convert.ToInt32(args[4]), client.Player.Heading);
				return;
			}
			#endregion Jump to X Y Z
			#region Jump rel +/-X +/-Y +/-Z
			else if (args.Length == 5 && args[1] == "rel")
			{
				client.Player.MoveTo(client.Player.CurrentRegionID,
									 client.Player.X + Convert.ToInt32(args[2]),
									 client.Player.Y + Convert.ToInt32(args[3]),
									 client.Player.Z + Convert.ToInt32(args[4]),
									 client.Player.Heading);
				return;
			}
			#endregion Jump rel +/-X +/-Y +/-Z
			#region Jump to X Y Z RegionID
			else if (args.Length == 6 && args[1] == "to")
			{
				if (CheckExpansion(client, client, (ushort)Convert.ToUInt16(args[5])))
				{
					client.Player.MoveTo(Convert.ToUInt16(args[5]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]), Convert.ToInt32(args[4]), client.Player.Heading);
					return;
				}
				return;
			}
			#endregion Jump to X Y Z RegionID
			#region Jump PlayerName to X Y Z
			else if (args.Length == 6 && args[2] == "to")
			{
				GameClient clientc;
				clientc = WorldMgr.GetClientByPlayerName(args[1], false, true);
				if (clientc == null)
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.PlayerIsNotInGame", args[1]), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				clientc.Player.MoveTo(clientc.Player.CurrentRegionID, Convert.ToInt32(args[3]), Convert.ToInt32(args[4]), Convert.ToInt32(args[5]), clientc.Player.Heading);
				return;
			}
			#endregion Jump PlayerName to X Y Z
			#region Jump PlayerName to X Y Z RegionID
			else if (args.Length == 7 && args[2] == "to")
			{
				GameClient clientc;
				clientc = WorldMgr.GetClientByPlayerName(args[1], false, true);
				if (clientc == null)
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.PlayerIsNotInGame", args[1]), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				if (CheckExpansion(clientc, clientc, (ushort)Convert.ToUInt16(args[6])))
				{
					clientc.Player.MoveTo(Convert.ToUInt16(args[6]), Convert.ToInt32(args[3]), Convert.ToInt32(args[4]), Convert.ToInt32(args[5]), clientc.Player.Heading);
					return;
				}
				return;
			}
			#endregion Jump PlayerName to X Y Z RegionID
			#region Jump PlayerName to PlayerCible
			else if (args.Length == 4 && args[2] == "to")
			{
				GameClient clientc;
				GameClient clientto;
				clientc = WorldMgr.GetClientByPlayerName(args[1], false, true);
				if (clientc == null)
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.PlayerIsNotInGame", args[1]), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				if (args[3] == "me")
				{
					clientto = client;
				}
				else
				{
					clientto = WorldMgr.GetClientByPlayerName(args[3], false, false);
				}

				if (clientto == null)
				{
					client.Out.SendMessage(LanguageMgr.GetTranslation(client, "GMCommands.Jump.PlayerIsNotInGame", args[3]), eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
				else
				{
					if (CheckExpansion(clientto, clientc, clientto.Player.CurrentRegionID))
					{
						if (clientto.Player.CurrentHouse != null)
							clientto.Player.CurrentHouse.Enter(clientc.Player);
						else
							clientc.Player.MoveTo(clientto.Player.CurrentRegionID, clientto.Player.X, clientto.Player.Y, clientto.Player.Z, client.Player.Heading);
						return;
					}
					return;
				}
			}
			#endregion Jump PlayerName to PlayerCible
			#region DisplaySyntax
			else
			{
				DisplaySyntax(client);
				return;
			}
			#endregion DisplaySyntax
		}

		public bool CheckExpansion(GameClient clientJumper, GameClient clientJumpee, ushort RegionID)
		{
			Region reg = WorldMgr.GetRegion(RegionID);
			if (reg != null && reg.Expansion > (int)clientJumpee.ClientType)
			{
				clientJumper.Out.SendMessage(LanguageMgr.GetTranslation(clientJumper, "GMCommands.Jump.CheckExpansion.CannotJump", clientJumpee.Player.Name, reg.Description), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				if (clientJumper != clientJumpee)
					clientJumpee.Out.SendMessage(LanguageMgr.GetTranslation(clientJumpee, "GMCommands.Jump.CheckExpansion.ClientNoSup", clientJumper.Player.Name, reg.Description), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return false;
			}
			return true;
		}
	}
}