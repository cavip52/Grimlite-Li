﻿using Grimoire.Game;
using System.Threading.Tasks;

namespace Grimoire.Botting.Commands.Misc.Statements
{
	public class CmdTargetAuraGreaterThan: StatementCommand, IBotCommand
	{
		public CmdTargetAuraGreaterThan()
		{
			Tag = "Aura";
			Text = "Target aura value greater than";
		}

		public Task Execute(IBotEngine instance)
		{
			string Aura = instance.IsVar(Value1) ? Configuration.Tempvariable[instance.GetVar(Value1)] : Value1;
			string AuraValue = instance.IsVar(Value2) ? Configuration.Tempvariable[instance.GetVar(Value2)] : Value2;

			int auraValue = Player.GetAuras(false, Aura);
			int x = 0;
			int.TryParse(AuraValue, out x);
			if (auraValue <= x)
				instance.Index++;
			return Task.FromResult<object>(null);
		}

		public override string ToString()
		{
			return $"Target aura greater than: {Value1}, {Value2}";
		}
	}
}
