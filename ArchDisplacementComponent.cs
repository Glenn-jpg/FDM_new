using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
namespace FDM
{
    public class ArchDisplacementComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ArchDisplacementComponent class.
        /// </summary>
        public ArchDisplacementComponent()
          : base("ArchDisplacementComponent2", "Arch D",
              "Finds displacement of a Nurbs Curve using ML",
              "Form Finding", "Machine Learning")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("XCoord", "X", "X-coord of control point", GH_ParamAccess.item); //0
            pManager.AddNumberParameter("YCoord", "Y", "Y-coord of control point", GH_ParamAccess.item); //1
            pManager.AddNumberParameter("RightCtrlPt", "Xright", "X-coord of right control point", GH_ParamAccess.item); //2
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Displacement[cm]", "Disp", "Displacement of beam", GH_ParamAccess.item); //0
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double xCoord = 0;
            double yCoord = 0;
            double RightxCoord = 0;

            DA.GetData(0, ref xCoord);
            DA.GetData(1, ref yCoord);
            DA.GetData(2, ref RightxCoord);

            float fxCoord = (float)xCoord;
            float fyCoord = (float)yCoord;
            float fRightxCoord = (float)RightxCoord;

            ModelInput sampleData = new ModelInput()
            {
                XCtrlPt_m_ = fxCoord,
                YCtrlPt_m_ = fyCoord,
                RightCtrlPt_m_ = fRightxCoord,
              
            };

            var predictionResult = ConsumeModel.Predict(sampleData);
            decimal res = new decimal(predictionResult.Score);
            double result = (double)res;
            DA.SetData(0, result);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("412aa6ec-0836-47a6-882b-0f317d97900a"); }
        }
    }
}