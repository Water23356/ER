namespace ER.STG
{
    public interface ILineGetter
    {
        public void ClearAllLine();

        public void AddLine(Line newLine);

        public void SetLines(Line[] newLines);
    }
}