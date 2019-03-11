using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class CliffordAttractor : StrangeAttractorBase
{
	[SerializeField]
	float a = 1.5f;
	[SerializeField]
	float b = -1.8f;
	[SerializeField]
	float c = 1.6f;
	[SerializeField]
	float d = 2.0f;

	int aId, bId, cId, dId;
	string aProp = "a", bProp = "b", cProp = "c", dProp = "d";

	protected sealed override void Initialize()
	{
		Assert.IsTrue(computeShader.name == "CliffordAttractor", "Please set CliffordAttractor.compute");
		base.Initialize();
	}

	protected sealed override void InitializeComputeBuffer()
	{
		if (cBuffer != null)
		{
			ReleaseBuffer(ref cBuffer);
		}

		cBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf(typeof(Params)));
		var parameters = new Params[instanceCount];

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
		aId = Shader.PropertyToID(aProp);
		bId = Shader.PropertyToID(bProp);
		cId = Shader.PropertyToID(cProp);
		dId = Shader.PropertyToID(dProp);
	}

	protected sealed override void UpdateShaderUniforms()
	{
		computeShaderInstance.SetFloat(aId, a);
		computeShaderInstance.SetFloat(bId, b);
		computeShaderInstance.SetFloat(cId, c);
		computeShaderInstance.SetFloat(dId, d);
	}
}