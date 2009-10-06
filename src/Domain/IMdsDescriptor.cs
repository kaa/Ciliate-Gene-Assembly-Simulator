using System.Collections.Generic;

namespace Domain {
	public interface IMdsDescriptor : IEnumerable<IMds> {
		int Length { get; }
		bool IsSuccessful { get; }
		bool TryLd(int p, int q, out IMdsDescriptor result);
		bool TryHi1(int p, int q, out IMdsDescriptor result);
		bool TryHi2(int p, int q, out IMdsDescriptor result);
		bool TryDlad1(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad2(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad3(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad4(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad5(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad6(int p, int q, int p2, int q2, out IMdsDescriptor result);
		bool TryDlad7(int p, int q, int p2, int q2, out IMdsDescriptor result);
	}
}