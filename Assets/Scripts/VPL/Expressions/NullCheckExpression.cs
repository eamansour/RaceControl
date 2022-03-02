using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NullCheckExpression : Expression<bool>
{
    private static List<IRaceFlag> s_raceFlags = new List<IRaceFlag>();

    public void Construct(List<IRaceFlag> raceFlags)
    {
        s_raceFlags = raceFlags;
    }

    private void Start()
    {
        if (s_raceFlags.Count == 0)
        {
            s_raceFlags.AddRange(GameObject.FindObjectsOfType<RaceFlag>());
        }
    }

    public override bool EvaluateExpression()
    {
        string operandText = GetSelectedDropdownText(DropdownInput);
        bool result = (Environment.ContainsKey(operandText) && Environment[operandText] != null);

        if (operandText == "YellowFlag")
        {
            result = EvaluateFlag(operandText, true);
        }

        if (operandText == "RedFlag")
        {
            result = EvaluateFlag(operandText, false);
        }

        return result;
    }

    /// <summary>
    /// Determine whether a red/yellow flag is on the track, given as true/false for
    /// a yellow or a red flag, respectively.
    /// </summary>
    private bool EvaluateFlag(string opText, bool yellowFlag)
    {
        List<IRaceFlag> flags = yellowFlag
            ? s_raceFlags.Where(flag => flag.Flag == RaceFlag.FlagType.YellowFlag).ToList()
            : s_raceFlags.Where(flag => flag.Flag == RaceFlag.FlagType.RedFlag).ToList();

        return flags.Count > 0;
    }
}
