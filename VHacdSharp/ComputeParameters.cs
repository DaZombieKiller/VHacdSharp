namespace VHacdSharp
{
    public struct ComputeParameters
    {
        public double Concavity { get; set; }
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double MinHullVolume { get; set; }
        public uint Resolution { get; set; }
        public uint MaxHullVertices { get; set; }
        public uint PlaneDownsampling { get; set; }
        public uint HullDownsampling { get; set; }
        public bool PCA { get; set; } // todo: better name
        public DecompositionMode DecompositionMode { get; set; }
        public bool HullApproximation { get; set; }
        public bool OpenCLAcceleration { get; set; }
        public uint MaxHulls { get; set; }
        public bool ProjectHullVertices { get; set; }

        public static readonly ComputeParameters Default = new ComputeParameters
        {
            Resolution          = 100_000,
            Concavity           = .001,
            PlaneDownsampling   = 4,
            HullDownsampling    = 4,
            Alpha               = .05,
            Beta                = .05,
            PCA                 = false,
            DecompositionMode   = DecompositionMode.Voxel,
            MaxHullVertices     = 64,
            MinHullVolume       = .0001,
            HullApproximation   = true,
            OpenCLAcceleration  = true,
            MaxHulls            = 1024,
            ProjectHullVertices = true
        };
    }
}
