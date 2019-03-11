using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class ThomasAttractor : StrangeAttractorBase
{
	[SerializeField, Range(-0.21f, 0.21f)]
	float b = 0.208186f;

	int bId;
	string bProp = "b";

	protected sealed override void Initialize()
	{
		Assert.IsTrue(computeShader.name == "ThomasAttractor", "Please set ThomasAttractor.compute");
		base.Initialize();
	}

	protected sealed override void InitializeComputeBuffer()
	{
		ReleaseBuffer(ref cBuffer);

		cBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf(typeof(Params)));
		Params[] parameters = new Params[instanceCount];

		for (int i = 0; i < instanceCount; ++i)
		{
			var rs = Random.insideUnitSphere;
			var color = gradient.Evaluate(rs.magnitude);
			parameters[i] = new Params(rs * emitterSize, particleSize, color);
		}

		cBuffer.SetData(parameters);
	}

	protected sealed override void InitializeShaderUniforms()
	{
		bId = Shader.PropertyToID(bProp);
	}

	protected sealed override void UpdateShaderUniforms()
	{
		computeShaderInstance.SetFloat(bId, b);
	}
}