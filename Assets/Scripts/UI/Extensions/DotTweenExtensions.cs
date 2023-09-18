using DG.Tweening;
using UnityEngine;

namespace UI.Extentsions
{
    public static class DotTweenExtensions
    {
        public static Tween DoAlpha(this SpriteRenderer renderer, float toAlpha, float duration)
        {
            return DOTween.To(() => renderer.color.a, value => renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, value), toAlpha, duration);
        }
    }
}