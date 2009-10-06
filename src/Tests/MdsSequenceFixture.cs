using System;
using Domain;
using Xunit;

namespace Tests {
	public class MdsSequenceFixture {

		static LinqMdsDescriptor Parse(string str) {
			return (LinqMdsDescriptor)MdsDescriptorParser.Parse(str, new LinqMdsDescriptorFactory());
		}

		[Fact]
		public void CreateFromEnumerable() {
			var sequence = new LinqMdsDescriptor( new [] { new Mds(0,1), new Mds(1,0) } );
			Assert.Equal("(0,1)(1,0)",sequence.ToString());
		}
		[Fact]
		public void CreateFromString() {
			var seq = Parse("(0,1)(1,2)(2,0)");
			Assert.Equal("(0,1)(1,2)(2,0)",seq.ToString());
		}
		[Fact]
		public void CreateFromInvalidString() {
			Assert.Throws<FormatException>(() => Parse("(0,1)(1,2"));
		}
		[Fact]
		public void Formatting() {
			var sequence = Parse("(0,1)(1,0)");
			Assert.Equal("(0,1)(1,0)", sequence.ToString() );
		}

		public class IsSuccessful {
			[Fact]
			public void True_if_sequence_contains_mds_with_Start_and_End_pointers() {
				Assert.True(Parse("(b,e)").IsSuccessful);
				Assert.True(Parse("(1,2)(b,e)(3,5)").IsSuccessful);
				Assert.True(Parse("(1,2)(b,1,2,3,e)(3,5)").IsSuccessful);
			}
			[Fact]
			public void True_if_sequence_contains_mds_with_inverted_Start_and_End_pointers() {
				Assert.True(Parse("(-e,-b)").IsSuccessful);
				Assert.True(Parse("(1,2)(-e,-3,-2,-1,-b)(3,5)").IsSuccessful);
			}
			[Fact]
			public void False_if_no_mds_in_sequence_has_0_In_and_Out_pointers() {
				Assert.False(Parse("(1,2)").IsSuccessful);
				Assert.False(Parse("(1,2)(2,2,3,3)(3,5)").IsSuccessful);
			}
		}

		public class CanLd {
			[Fact]
			public void True_for_matching_In_and_Out_pointers() {
				var seq = Parse("(1,2)(2,3)");
				Assert.True(seq.CanLd(0, 1));
			}
			[Fact]
			public void True_for_matching_pointers_with_Mdss_between() {
				var seq = Parse("(1,2)(4,5)(2,3)");
				Assert.True(seq.CanLd(0, 2));
			}
			[Fact]
			public void False_for_mismatched_In_Out_pointers() {
				var seq = Parse("(2,1)(4,5)(2,3)");
				Assert.False(seq.CanLd(0, 2));
			}
			[Fact]
			public void False_for_negated_In_Out_pointers() {
				var seq = Parse("(1,-2)(4,5)(2,3)");
				Assert.False(seq.CanLd(0, 2));
			}
		}
		public class Ld {
			[Fact]
			public void Excises_bit_between_Mdss() {
				var seq = Parse("(0,1)(1,2)(5,6)(3,4)(2,3)(3,0)");
				Assert.Equal("(0,1)(1,2,3)(3,0)", seq.Ld(1, 4).ToString());
			}
			[Fact]
			public void Merges_content() {
				var seq = Parse("(0,1,2)(2,3,0)");
				Assert.Equal("(0,1,2,3,0)", seq.Ld(0, 1).ToString());
			}
			[Fact]
			public void Preserves_inversion() {
				var seq = Parse("(-1,-2)(-2,-3)");
				Assert.Equal("(-1,-2,-3)", seq.Ld(0, 1).ToString());
			}
		}

		public class CanHi {
			[Fact]
			public void True_for_matching_negated_pointers() {
				Assert.True(Parse("(-2,1)(2,3)").CanHi1(0, 1), "§1");
				Assert.True(Parse("(1,2)(3,-2)").CanHi2(0, 1), "§2");
			}
			[Fact]
			public void False_for_mismatched_negated_pointers() {
				Assert.False(Parse("(2,3)(-3,4)").CanHi1(0, 1),"§1");
				Assert.False(Parse("(1,2)(4,-3)").CanHi2(0, 1),"§2");
			}
			[Fact]
			public void False_for_matching_unnegated_pointers() {
				Assert.False(Parse("(2,1)(3,2)").CanHi1(0, 1), "§1");
				Assert.False(Parse("(1,2)(3,2)").CanHi2(0, 1), "§2");
			}
		}

		public class Hi {
			[Fact]
			public void Merges_involved_content() {
				Assert.Equal("(-1,-2,-3,-4,-5)", Parse("(3,2,1)(-3,-4,-5)").Hi1(0, 1).ToString());
				Assert.Equal("(1,2,3,4,5)", Parse("(1,2,3)(-5,-4,-3)").Hi2(0, 1).ToString());
			}
			[Fact]
			public void Preserves_surrounding_context() {
				Assert.Equal("(5,6)(-1,-2,-3)(-3,-4)", Parse("(5,6)(2,1)(-2,-3)(-3,-4)").Hi1(1, 2).ToString());
				Assert.Equal("(5,6)(1,2,3)(-3,-4)", Parse("(5,6)(1,2)(-3,-2)(-3,-4)").Hi2(1, 2).ToString());
			}
			[Fact]
			public void Hi1_inverses_intermediate_context_before_merge() {
				Assert.Equal("(5,6)(-8,-7)(-7,-6)(-1,-2,-3)(-3,-4)", Parse("(5,6)(2,1)(6,7)(7,8)(-2,-3)(-3,-4)").Hi1(1, 4).ToString());
			}
			[Fact]
			public void Hi2_inverses_intermediate_content_after_merge() {
				Assert.Equal("(5,6)(1,2,3)(-8,-7)(-7,-6)(-3,-4)", Parse("(5,6)(1,2)(6,7)(7,8)(-3,-2)(-3,-4)").Hi2(1, 4).ToString());
			}
		}

		public class CanDlad {
			[Fact]
			public void True_for_matching_InInOutOut_pointers() {
				Assert.True(Parse("(2,3)(4,5)(1,2)(3,4)").CanDlad1(0, 1, 2, 3));
			}
			[Fact]
			public void True_for_matching_InOutOutIn_pointers() {
				Assert.True(Parse("(2,3)(3,4)(1,2)(4,5)").CanDlad2(0, 1, 2, 3));
			}
			[Fact]
			public void True_for_matching_OutInInOut_pointers() {
				Assert.True(Parse("(1,2)(4,5)(2,3)(3,4)").CanDlad3(0, 1, 2, 3));
			}
			[Fact]
			public void True_for_matching_OutOutInIn_pointers() {
				Assert.True(Parse("(1,2)(3,4)(2,3)(4,5)").CanDlad4(0, 1, 2, 3));
			}
			[Fact]
			public void True_for_matching_InSharedOut_pointers() {
				Assert.True(Parse("(3,4)(2,3)(1,2)").CanDlad5(0, 1, 1, 2));
			}
			[Fact]
			public void True_for_matching_SharedOutIn_pointers() {
				Assert.True(Parse("(2,3)(1,2)(3,4)").CanDlad6(0, 0, 1, 2));
			}
			[Fact]
			public void False_for_matching_SharedInOut_pointers() {
				Assert.False(Parse("(2,3)(3,4)(1,2)").CanDlad1(0, 0, 1, 2));
			}
			[Fact]
			public void True_for_matching_OutInShared_pointers() {
				Assert.True(Parse("(1,2)(3,4)(2,3)").CanDlad7(0, 1, 2, 2));
			}
			[Fact]
			public void False_for_p_q_q2_as_overlapping_pointers() {
				Assert.False(Parse("(1,1)(1,1)(1,1)").CanDlad7(0, 2, 2, 2));
			}
			[Fact]
			public void False_for_p_q_p2_as_overlapping_pointers() {
				Assert.False(Parse("(1,1)(1,1)(1,1)").CanDlad7(0, 0, 0, 2));
			}
		}

		public class Dlad {
			[Fact]
			public void InInOutOut_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3)(3,4,5,6,7)(7,8)(8,9,10,11,12)(12,13)(13,14)",
					Parse("(1,2)(10,11,12)(12,13)(5,6,7)(7,8)(8,9,10)(2,3)(3,4,5)(13,14)").Dlad1(1, 3, 5, 7).ToString());
			}
			[Fact]
			public void InOutOutIn_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3)(3,4)(4,5,6,7,8)(8,9)(9,10,11,12,13)(13,14)",
					Parse("(1,2)(6,7,8)(8,9)(9,10,11)(3,4)(4,5,6)(2,3)(11,12,13)(13,14)").Dlad2(1, 3, 5, 7).ToString());
			}
			[Fact]
			public void OutInInOut_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3,4,5,6)(6,7)(7,8,9,10,11)(11,12)(12,13)(13,14)",
					Parse("(1,2)(2,3,4)(12,13)(9,10,11)(11,12)(4,5,6)(6,7)(7,8,9)(13,14)").Dlad3(1, 3, 5, 7).ToString());
			}
			[Fact]
			public void OutOutInIn_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3,4,5,6)(6,7)(7,8)(8,9)(9,10,11,12,13)(13,14)",
					Parse("(1,2)(2,3,4)(8,9)(9,10,11)(7,8)(4,5,6)(6,7)(11,12,13)(13,14)").Dlad4(1, 3, 5, 7).ToString());
			}
			[Fact]
			public void InSharedOut_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3)(3,4,5,6,7,8,9)(9,10)(10,11)",
					Parse("(1,2)(7,8,9)(9,10)(5,6,7)(2,3)(3,4,5)(10,11)").Dlad5(1, 3, 3, 5).ToString());
			}
			[Fact]
			public void SharedOutIn_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3)(3,4)(4,5,6,7,8,9,10)(10,11)",
					Parse("(1,2)(6,7,8)(3,4)(4,5,6)(2,3)(8,9,10)(10,11)").Dlad6(1, 1, 3, 5).ToString());
			}
			[Fact]
			public void OutInShared_pattern_is_assembled_correctly() {
				Assert.Equal(
					"(1,2)(2,3,4,5,6,7,8)(8,9)(9,10)(10,11)",
					Parse("(1,2)(2,3,4)(9,10)(6,7,8)(8,9)(4,5,6)(10,11)").Dlad7(1, 3, 5, 5).ToString());
			}
		}
	}
}
