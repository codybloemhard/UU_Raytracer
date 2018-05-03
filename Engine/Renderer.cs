using Engine.TemplateCode;

namespace Engine
{
   public interface IRenderer
   {
      void Render(Surface surface, Scene scene);
   }
}