using System.Collections.Generic;

namespace Domain {
	public interface IMdsDescriptor : IEnumerable<Mds> {
		IMds this[int index] { get; }
		int Length { get; }
		bool IsSuccessful { get; }
		bool CanLd(int p, int q);
		bool TryLd(int p, int q, out IMdsDescriptor result);
		IMdsDescriptor Ld(int p, int q);
		bool CanHi(int p, int q);
		bool CanHi1(int p, int q);
		bool CanHi2(int p, int q);
		bool TryHi(int p, int q, out IMdsDescriptor result );
		IMdsDescriptor Hi(int p, int q);
		IMdsDescriptor Hi1(int p, int q);
		IMdsDescriptor Hi2(int p, int q);
		bool CanDlad(int p, int q, int p2, int q2 );
		bool CanDlad1(int p, int q, int p2, int q2);
		bool CanDlad2(int p, int q, int p2, int q2);
		bool CanDlad3(int p, int q, int p2, int q2);
		bool CanDlad4(int p, int q, int p2, int q2);
		bool CanDlad5(int p, int q, int p2, int q2);
		bool CanDlad6(int p, int q, int p2, int q2);
		bool CanDlad7(int p, int q, int p2, int q2);
		bool TryDlad(int p, int q, int p2, int q2, out IMdsDescriptor result);
		IMdsDescriptor Dlad(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad1(int p, int q, int p2, int q2 );
		IMdsDescriptor Dlad2(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad3(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad4(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad5(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad6(int p, int q, int p2, int q2);
		IMdsDescriptor Dlad7(int p, int q, int p2, int q2);
	}
}