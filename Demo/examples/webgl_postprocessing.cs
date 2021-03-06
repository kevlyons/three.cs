﻿namespace Demo.examples
{
    using System.Drawing;
    using System.Windows.Forms;

    using Examples;

    using THREE;

    using ThreeCs.Cameras;
    using ThreeCs.Core;
    using ThreeCs.Extras.Geometries;
    using ThreeCs.Lights;
    using ThreeCs.Materials;
    using ThreeCs.Math;
    using ThreeCs.Objects;
    using ThreeCs.Scenes;

    [Example("webgl_postprocessing", ExampleCategory.OpenTK, "postprocessing", 0.4f)]
    class webgl_postprocessing : Example
    {
        private PerspectiveCamera camera;

        private Scene scene;

        private EffectComposer composer;

        private Light light;

        private Object3D object3D;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public override void Load(Control control)
        {
            base.Load(control);

            camera = new PerspectiveCamera(70, control.Width / (float)control.Height, 1, 1000);
            this.camera.Position.Z = 400;

            scene = new Scene();
            scene.Fog = new Fog(Color.Black, 1, 1000);

            object3D = new Object3D();
            scene.Add(object3D);

            var geometry = new SphereGeometry(1, 4, 4);
			var material = new MeshPhongMaterial() { Color = Color.White, Shading = ThreeCs.Three.FlatShading };

            for (var i = 0; i < 100; i++)
            {
                var mesh = new Mesh(geometry, material);
                mesh.Position.set(Mat.Random() - 0.5f, Mat.Random() - 0.5f, Mat.Random() - 0.5f).Normalize();
                mesh.Position.MultiplyScalar(Mat.Random() * 400);
                mesh.Rotation.set(Mat.Random() * 2, Mat.Random() * 2, Mat.Random() * 2);
                mesh.Scale.X = mesh.Scale.Y = mesh.Scale.Z = Mat.Random() * 50;
                object3D.Add(mesh);
            }

            scene.Add(new AmbientLight((Color)colorConvertor.ConvertFromString("#222222")));

            light = new DirectionalLight(Color.White);
            light.Position.set(1, 1, 1);
            scene.Add(light);

			// postprocessing

            composer = new EffectComposer(renderer, control);
            composer.AddPass(new RenderPass(scene, camera));

            var effect1 = new ShaderPass(new DotScreenShader());
            effect1.Uniforms["scale"]["value"] = 4;
            composer.AddPass(effect1);

            var effect2 = new ShaderPass(new RGBShiftShader());
            effect2.Uniforms["amount"]["value"] = 0.0015f;
            effect2.RenderToScreen = true;
            composer.AddPass(effect2);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Render()
        {
            object3D.Rotation.X += 0.005f;
            object3D.Rotation.Y += 0.01f;

            composer.Render();
       //     renderer.Render(this.scene, this.camera);
        }
    }
}
