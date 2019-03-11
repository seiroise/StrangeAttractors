using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class LorenzAttractor : StrangeAttractorBase
{
	[SerializeField]
	float p = 10f;
	[SerializeField]
	float r = 20f;
	[SerializeField]
	float b = 2.6666667f;

	int pId, rId, bId;
	string pProp = "p", rProp = "r", bProp = "b";

	protected sealed override void Initialize()
	{
		Assert.IsTrue(computeShader.name == "LorenzAttractor", "Please set LorenzAttractor.compute");
		base.Initialize();
	}

	protected sealed override void InitializeComputeBuffer()
	{
		if (cBuffer != null)
		{
			ReleaseBuffer(ref cBuffer);
		}

		cBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf(typeof(Params)));
		var parameters = new Params[cBuffer.count];
		for (int i = 0; i < instanceCount; ++i)
		{
			var normalize = (float)i / instanceCount;
			var color = gradient.Evaluate(normalize);
			parameters[i] = new Params(Random.insideUnitSphere * emitterSize * normalize, particleSize, color);
		}
		cBuffer.SetData(parameters);
	}

	protected sealed override void InitializeShaderUniforms()
	{
		pId = Shader.PropertyToID(pProp);
		rId = Shader.PropertyToID(rProp);
		bId = Shader.PropertyToID(bProp);
	}

	protected sealed override void UpdateShaderUniforms()
	{
		computeShaderInstance.SetFloat(pId, p);
		computeShaderInstance.SetFloat(rId, r);
		computeShaderInstance.SetFloat(bId, b);
	}
}