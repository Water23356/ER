namespace ER.InputSolution
{
    public interface InputController
    {
        public void UpdateMonitor(MonitorFrameInfo mfi);

        public InputRecorder Recorder { get; set; }
    }
}