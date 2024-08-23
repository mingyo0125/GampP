using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Globalization;

public static class ExtensionMethod
{
    public static Coroutine TextAnimation(this TextMeshProUGUI text, bool isRepeat = false)
    {
        return text.StartCoroutine(TextAnimationCorou(text, isRepeat));
    }

    private static IEnumerator TextAnimationCorou(TextMeshProUGUI text, bool isRepeat)
    {
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;

        while (isRepeat)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) { continue; }

                Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                Vector3[] originalVertices = new Vector3[4];
                for (int j = 0; j < 4; j++)
                {
                    originalVertices[j] = vertices[charInfo.vertexIndex + j];
                }

                Sequence sequence = DOTween.Sequence();
                sequence.Append(DOTween.To(
                    () => 0f,
                    y =>
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            vertices[charInfo.vertexIndex + j] = originalVertices[j] + new Vector3(0, y, 0);
                        }
                        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                    },
                    10f, // 올라가는 높이
                    0.1f // 올라가는 시간
                ).SetEase(Ease.OutQuad));

                sequence.Append(DOTween.To(
                    () => 10f,
                    y =>
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            vertices[charInfo.vertexIndex + j] = originalVertices[j] + new Vector3(0, y, 0);
                        }
                        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                    },
                    0f, // 내려오는 높이
                    0.1f // 내려오는 시간
                ).SetEase(Ease.InQuad));

                sequence.Play();

                yield return new WaitForSeconds(0.4f);

                sequence.Kill();
            }

            yield return null;
        }
    }
}
