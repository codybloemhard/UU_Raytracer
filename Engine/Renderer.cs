using Engine.TemplateCode;

namespace Engine
{
   public interface IRenderer<T>
   {
      void Render(Surface surface, T scene);
   }
}