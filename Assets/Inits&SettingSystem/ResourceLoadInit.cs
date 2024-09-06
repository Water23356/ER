using ER.Resource;
using UnityEngine;


/// <summary>
/// 初始化器, 顺序:1
/// </summary>
public class ResourceLoadInit : MonoBehaviour
{
    public LoadProgressPanel progressPanel;
    private LoadTask task;

    public void Load(string path)
    {
        task = GR.Get<LoadPackResource>(path).task; 
        GR.AddLoadTask(task);
        enabled = true;
    }

    private void Update()
    {
        if(task!=null)
        {
            float sum = task.progress_load.count + task.progress_load_force.count;
            int count = task.progress_load.loaded + task.progress_load_force.loaded;

            progressPanel.SetLoadTxt($"{count}/{sum}");
            progressPanel.SetProgressPart(count/sum);

            //Debug.Log($"{count}/{sum}");
            if(task.progress_load.done && task.progress_load_force.done)
            {
                enabled = false;
                InitMonitor.InitCallback();
            }
        }
    }



}
