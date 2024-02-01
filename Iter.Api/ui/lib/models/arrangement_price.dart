import 'package:json_annotation/json_annotation.dart';

part 'arrangement_price.g.dart';

@JsonSerializable()
class ArrangementPrice {
  String? accommodationType;
  double? price;

  ArrangementPrice({this.accommodationType, this.price});

  factory ArrangementPrice.fromJson(Map<String, dynamic> json) =>
      _$ArrangementPriceFromJson(json);
  Map<String, dynamic> toJson() => _$ArrangementPriceToJson(this);
}
