using UnityEngine;
using Unity.Sentis;

public class NumberReader : MonoBehaviour
{
    
    public ModelAsset modelAsset;

    Model runtimeModel;
    Worker worker;
    public float[] results;

    void Start()
    {
        Model sourceModel = ModelLoader.Load(modelAsset);

        // Create a functional graph that runs the input model and then applies softmax to the output.
        FunctionalGraph graph = new FunctionalGraph();
        FunctionalTensor[] inputs = graph.AddInputs(sourceModel);
        FunctionalTensor[] outputs = Functional.Forward(sourceModel, inputs);
        FunctionalTensor softmax = Functional.Softmax(outputs[0]);
        var indexOfMaxProba = Functional.ArgMax(softmax, -1, false);


        runtimeModel = graph.Compile(softmax, indexOfMaxProba);

    }
    public async void Execute(Texture2D inputTexture)
    {
    using Tensor inputTensor = TextureConverter.ToTensor(inputTexture,  channels: 3,width: 224, height: 224);

        // Create an engine
        worker = new Worker(runtimeModel, BackendType.GPUCompute);

        // Run the model with the input data
        worker.Schedule(inputTensor);

        // Get the result
        Tensor<float> outputTensor = worker.PeekOutput() as Tensor<float>;

        // outputTensor is still pending
        // Either read back the results asynchronously or do a blocking download call
        results = outputTensor.DownloadToArray();
        using var probabilities = (worker.PeekOutput(0) as Tensor<float>).ReadbackAndClone();
        using var indexOfMaxProba = (worker.PeekOutput(1) as Tensor<int>).ReadbackAndClone();

        var predictedNumber = indexOfMaxProba[0];
        var probability = probabilities[predictedNumber];

        Debug.Log(predictedNumber);
        
    

        
    }

    void OnDisable()
    {
        // Tell the GPU we're finished with the memory the engine used
        worker.Dispose();
    }
}