import 'package:json_annotation/json_annotation.dart';

part 'arrangement_earnings.g.dart';

@JsonSerializable()

class ArrangementEarnings {
  double? totalPaid;
  int reservationCount;
  String? arrangementId;
  String? arrangementName;

  ArrangementEarnings({
    this.totalPaid,
    required this.reservationCount,
    this.arrangementId,
    this.arrangementName,
  });

      factory ArrangementEarnings.fromJson(Map<String, dynamic> json) =>
      _$ArrangementEarningsFromJson(json);

  Map<String, dynamic> toJson() => _$ArrangementEarningsToJson(this);
}