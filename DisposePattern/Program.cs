using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DisposePattern
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    // Dispose with TemplateMethod
    public class ProperComplexResourceHolder : IDisposable
    {
        // Буфер из неуправляемого кода 
        private IntPtr _buffer;
        // Дескриптор событий ОС (управляемый рессурс)
        private SafeHandle _handle;

        public ComplexResourceHolder()
        {
            // Захватываем ресурсы
            _buffer = AllocateBuffer();
            _handle = new SafeWaitHandle(IntPtr.Zero, true);
        }

        protected virtual void DisposeManagedResources()
        {
            if (_handle != null)
            {
                _handle.Dispose();
            }
        }

        ~ProperComplexResourceHolder()
        {
            DisposeNativeResources();
        }

        private void DisposeNativeResources()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            DisposeNativeResources();
            DisposeManagedResources();
            GC.SuppressFinalize(this);
        }
    }





    // Избыточен в 99,999 случаев
    // Паттерн работает только если в классе есть управляемые и неуправляемые ресурсы
    public class ComplexResourceHolder : IDisposable
    {
        // Буфер из неуправляемого кода 
        private IntPtr _buffer;
        // Дескриптор событий ОС (управляемый рессурс)
        private SafeHandle _handle;

        public ComplexResourceHolder()
        {
            // Захватываем ресурсы
            _buffer = AllocateBuffer();
            _handle = new SafeWaitHandle(IntPtr.Zero, true);
        }
        protected virtual void Dispose (bool disposing)
        {
            // Неуправляемые ресурсы  освобождаются в любом случае
            ReleaseBuffer(_buffer);
            // Вызываем из метода Dispose, освобождаем управляемые ресурсы
            if (disposing)
            {
                if (_handle != null)
                {
                    _handle.Dispose();
                }
            }
        }

        ~ComplexResourceHolder()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
