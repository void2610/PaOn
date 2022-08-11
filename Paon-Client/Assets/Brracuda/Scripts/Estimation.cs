using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

namespace Estimate
{
    public class Estimation : MonoBehaviour
    {
        [SerializeField]
        NNModel _model = null;

        [SerializeField]
        WebcamInput _webcam = null;

        [Space]
        [SerializeField]
        Mesh _jointMesh = null;

        [SerializeField]
        Mesh _boneMesh = null;

        [Space]
        [SerializeField]
        Material _jointMaterial = null;

        [SerializeField]
        Material _boneMaterial = null;

        [Space]
        [SerializeField]
        RawImage _monitorUI = null;

        static readonly (int, int)[] BonePairs =
                {
                    (0, 1),
                    (1, 2),
                    (1, 2),
                    (2, 3),
                    (3, 4), // Thumb
                    (5, 6),
                    (6, 7),
                    (7, 8), // Index finger
                    (9, 10),
                    (10, 11),
                    (11, 12), // Middle finger
                    (13, 14),
                    (14, 15),
                    (15, 16), // Ring finger
                    (17, 18),
                    (18, 19),
                    (19, 20), // Pinky
                    (0, 17),
                    (2, 5),
                    (5, 9),
                    (9, 13),
                    (13, 17) // Palm
                };

        private Model _runtimeModel = null;

        private IWorker _worker = null;

        // Start is called before the first frame update
        void Start()
        {
            _runtimeModel = ModelLoader.Load(_model);
            _worker =
                WorkerFactory
                    .CreateWorker(WorkerFactory.Type.Compute, _runtimeModel);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
