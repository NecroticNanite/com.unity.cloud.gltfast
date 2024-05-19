// SPDX-FileCopyrightText: 2023 Unity Technologies and the glTFast authors
// SPDX-License-Identifier: Apache-2.0

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace GLTFast
{

    using Schema;

    /// <summary>
    /// Extension methods for <see cref="NodeBase"/>
    /// </summary>
    public static class NodeExtension
    {

        /// <summary>
        /// Get translation, rotation and scale of a node, regardless of source
        /// properties
        /// </summary>
        /// <param name="node">Input node</param>
        /// <param name="position">Node's translation</param>
        /// <param name="rotation">Node's rotation</param>
        /// <param name="scale">Node's scale</param>
        public static void GetTransform(this NodeBase node, out double3 position, out Quaternion rotation, out Vector3 scale)
        {

            position = double3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;

            if (node.matrix != null)
            {
                Matrix4x4 m = new Matrix4x4();
                m.m00 = (float)node.matrix[0];
                m.m10 = (float)-node.matrix[1];
                m.m20 = (float)-node.matrix[2];
                m.m30 = (float)node.matrix[3];
                m.m01 = (float)-node.matrix[4];
                m.m11 = (float)node.matrix[5];
                m.m21 = (float)node.matrix[6];
                m.m31 = (float)node.matrix[7];
                m.m02 = (float)-node.matrix[8];
                m.m12 = (float)node.matrix[9];
                m.m22 = (float)node.matrix[10];
                m.m32 = (float)node.matrix[11];
                m.m03 = (float)-node.matrix[12];
                m.m13 = (float)node.matrix[13];
                m.m23 = (float)node.matrix[14];
                m.m33 = (float)node.matrix[15];

                var doubleTranslate = new double3(node.matrix[12], node.matrix[13], node.matrix[14]);
                m.Decompose(out var t, out var r, out var s);
                position = doubleTranslate;
                rotation = r;
                scale = s;

            }
            else
            {
                if (node.translation != null)
                {
                    Assert.AreEqual(node.translation.Length, 3);
                    position = new double3(
                        -node.translation[0],
                        node.translation[1],
                        node.translation[2]
                    );
                }
                if (node.rotation != null)
                {
                    Assert.AreEqual(node.rotation.Length, 4);
                    rotation = new Quaternion(
                        node.rotation[0],
                        -node.rotation[1],
                        -node.rotation[2],
                        node.rotation[3]
                    );
                }
                if (node.scale != null)
                {
                    Assert.AreEqual(node.scale.Length, 3);
                    scale = new Vector3(
                        (float)node.scale[0],
                        (float)node.scale[1],
                        (float)node.scale[2]
                    );
                }
            }
        }
    }
}
