using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public static class ExtensionMethod
{
    public static Coroutine TextAnimation(this TextMeshProUGUI texts, bool isRepeat = false)
    {
        return texts.StartCoroutine(TextAnimationCorou(texts, isRepeat));
    }

    private static IEnumerator TextAnimationCorou(TextMeshProUGUI texts, bool isRepeat)
    {
        texts.ForceMeshUpdate();
        TMP_TextInfo textInfo = texts.textInfo;

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
                        texts.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                    },
                    10f, // �ö󰡴� ����
                    0.1f // �ö󰡴� �ð�
                ).SetEase(Ease.OutQuad));

                sequence.Append(DOTween.To(
                    () => 10f,
                    y =>
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            vertices[charInfo.vertexIndex + j] = originalVertices[j] + new Vector3(0, y, 0);
                        }
                        texts.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                    },
                    0f, // �������� ����
                    0.1f // �������� �ð�
                ).SetEase(Ease.InQuad));

                sequence.Play();

                yield return new WaitForSeconds(0.4f);

                sequence.Kill();
            }

            yield return null;
        }
    }
}
