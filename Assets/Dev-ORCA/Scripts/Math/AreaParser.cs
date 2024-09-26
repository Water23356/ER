using UnityEngine;

namespace Dev
{
    public class AreaParser
    {
        public void Parse(Vector2[] line, Vector2[] pos)
        {
            AreaLine[] areas = new AreaLine[line.Length];
            for (int i = 0; i < areas.Length; i++)
            {
                areas[i] = new AreaLine()
                {
                    dir = line[i],
                    node = pos[i],
                    state = AreaLine.LineState.None
                };
            }

            for (int i = 0; i < areas.Length; i++)
            {
                for (int k = i; k < areas.Length; k++)
                {

                }
            }
        }
    }
}