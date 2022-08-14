using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class HandEstimation : MonoBehaviour
{
    public NNModel _model;

    public ComputeShader _compute;

    public WorkerFactory.Type _workerType = WorkerFactory.Type.Auto;

    private struct Engine
    {
        public WorkerFactory.Type workerType;

        public IWorker worker;

        public Engine(WorkerFactory.Type workerType, Model model)
        {
            this.workerType = workerType;
            worker = WorkerFactory.CreateWorker(workerType, model);
        }
    }

    private Engine engine;

    private string heatmapLayer;

    private string offsetsLayer;

    private string predictionLayer = "predictLayer";

    private void InitEstimation()
    {
        Model runtimeModel;

        runtimeModel = ModelLoader.Load(_model);

        heatmapLayer = runtimeModel.outputs[0];
        offsetsLayer = runtimeModel.outputs[1];

        ModelBuilder modelBuilder = new ModelBuilder(runtimeModel);

        modelBuilder.Sigmoid (predictionLayer, heatmapLayer);

        engine = new Engine(_workerType, modelBuilder.model);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
